#include "TPSCharacter.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/SpringArmComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Net/UnrealNetwork.h"

ATPSCharacter::ATPSCharacter()
{
    SpringArm = CreateDefaultSubobject<USpringArmComponent>(TEXT("SpringArm"));
    SpringArm->SetupAttachment(GetRootComponent());
    SpringArm->TargetArmLength = 400.0f;
    SpringArm->bUsePawnControlRotation = true;

    Camera = CreateDefaultSubobject<UCameraComponent>(TEXT("Camera"));
    Camera->SetupAttachment(SpringArm, USpringArmComponent::SocketName);
    Camera->bUsePawnControlRotation = false;

    GunMesh = CreateDefaultSubobject<USkeletalMeshComponent>(TEXT("GunMesh"));
    GunMesh->SetupAttachment(GetMesh(), TEXT("WeaponSocket"));

    GetCharacterMovement()->MaxWalkSpeed = 500.0f;
    bIsAiming = false;
    bIsInCover = false;
    CurrentHealth = MaxHealth;
}

void ATPSCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (HasAuthority()) ServerSpawnWeapons();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
    {
        if (UEnhancedInputLocalPlayerSubsystem* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
    }
    DefaultFOV = Camera->FieldOfView;
}

void ATPSCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    Super::SetupPlayerInputComponent(PIC);
    if (UEnhancedInputComponent* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ATPSCharacter::Move);
        EIC->BindAction(LookAction, ETriggerEvent::Triggered, this, &ATPSCharacter::Look);
        EIC->BindAction(JumpAction, ETriggerEvent::Started, this, &ATPSCharacter::StartJump);
        EIC->BindAction(FireAction, ETriggerEvent::Started, this, &ATPSCharacter::StartFire);
        EIC->BindAction(FireAction, ETriggerEvent::Completed, this, &ATPSCharacter::StopFire);
        EIC->BindAction(ReloadAction, ETriggerEvent::Started, this, &ATPSCharacter::Reload);
        EIC->BindAction(AimAction, ETriggerEvent::Started, this, &ATPSCharacter::StartAim);
        EIC->BindAction(AimAction, ETriggerEvent::Completed, this, &ATPSCharacter::StopAim);
        EIC->BindAction(CoverAction, ETriggerEvent::Started, this, &ATPSCharacter::StartCover);
        EIC->BindAction(CoverAction, ETriggerEvent::Completed, this, &ATPSCharacter::StopCover);
        EIC->BindAction(SwitchWeaponAction, ETriggerEvent::Triggered, this, &ATPSCharacter::SwitchWeapon);
    }
}

void ATPSCharacter::Move(const FInputActionValue& Value)
{
    FVector2D V = Value.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), V.Y);
    AddMovementInput(GetActorRightVector(), V.X);
}

void ATPSCharacter::Look(const FInputActionValue& Value)
{
    FVector2D V = Value.Get<FVector2D>();
    AddControllerYawInput(V.X);
    AddControllerPitchInput(V.Y);
}

void ATPSCharacter::StartJump() { Jump(); }
void ATPSCharacter::StartFire() { if (CurrentWeapon) CurrentWeapon->StartFire(); }
void ATPSCharacter::StopFire() { if (CurrentWeapon) CurrentWeapon->StopFire(); }
void ATPSCharacter::Reload() { if (CurrentWeapon) CurrentWeapon->Reload(); }

void ATPSCharacter::StartAim()
{
    bIsAiming = true;
    SpringArm->TargetArmLength = 200.0f;
    Camera->FieldOfView = 60.0f;
    GetCharacterMovement()->MaxWalkSpeed = 250.0f;
}

void ATPSCharacter::StopAim()
{
    bIsAiming = false;
    SpringArm->TargetArmLength = 400.0f;
    Camera->FieldOfView = DefaultFOV;
    GetCharacterMovement()->MaxWalkSpeed = 500.0f;
}

void ATPSCharacter::StartCover()
{
    if (!bIsInCover) { bIsInCover = true; GetCharacterMovement()->MaxWalkSpeed = 150.0f; }
}

void ATPSCharacter::StopCover()
{
    if (bIsInCover) { bIsInCover = false; GetCharacterMovement()->MaxWalkSpeed = 500.0f; }
}

void ATPSCharacter::SwitchWeapon(const FInputActionValue& Value)
{
    float Scroll = Value.Get<float>();
    if (Scroll != 0.0f && CurrentWeapon) CurrentWeapon->Destroy();
}

void ATPSCharacter::ServerSpawnWeapons_Implementation()
{
    if (WeaponClasses.Num() > 0)
    {
        FActorSpawnParameters P;
        P.Owner = this;
        P.Instigator = this;
        CurrentWeapon = GetWorld()->SpawnActor<ATPSWeapon>(WeaponClasses[0], FVector::ZeroVector, FRotator::ZeroRotator, P);
        if (CurrentWeapon)
        {
            CurrentWeapon->AttachToComponent(GunMesh, FAttachmentTransformRules::SnapToTargetNotIncludingScale);
            CurrentWeapon->SetOwner(this);
        }
    }
}

float ATPSCharacter::TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer)
{
    float Actual = Super::TakeDamage(Damage, Event, Instigator, Causer);
    CurrentHealth = FMath::Clamp(CurrentHealth - Actual, 0.0f, MaxHealth);
    if (CurrentHealth <= 0.0f && HasAuthority()) Destroy();
    return Actual;
}

void ATPSCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(ATPSCharacter, CurrentHealth);
    DOREPLIFETIME(ATPSCharacter, CurrentWeapon);
}
