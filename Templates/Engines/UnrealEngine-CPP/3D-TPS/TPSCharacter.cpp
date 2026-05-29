// Third-person shooter character with aiming, cover, weapons, and damage.
#include "TPSCharacter.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/SpringArmComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Net/UnrealNetwork.h"

// Set up spring arm camera, gun mesh, movement, and initial health.
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

// Bind all enhanced input actions for TPS controls.
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

// Handle movement input.
void ATPSCharacter::Move(const FInputActionValue& Value)
{
    FVector2D V = Value.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), V.Y);
    AddMovementInput(GetActorRightVector(), V.X);
}

// Handle mouse look input.
void ATPSCharacter::Look(const FInputActionValue& Value)
{
    FVector2D V = Value.Get<FVector2D>();
    AddControllerYawInput(V.X);
    AddControllerPitchInput(V.Y);
}

// Start jump.
void ATPSCharacter::StartJump() { Jump(); }
// Start firing current weapon.
void ATPSCharacter::StartFire() { if (CurrentWeapon) CurrentWeapon->StartFire(); }
// Stop firing current weapon.
void ATPSCharacter::StopFire() { if (CurrentWeapon) CurrentWeapon->StopFire(); }
// Reload current weapon.
void ATPSCharacter::Reload() { if (CurrentWeapon) CurrentWeapon->Reload(); }

// Enter aiming state: shorten spring arm, zoom FOV, slow walk speed.
void ATPSCharacter::StartAim()
{
    bIsAiming = true;
    SpringArm->TargetArmLength = 200.0f;
    Camera->FieldOfView = 60.0f;
    GetCharacterMovement()->MaxWalkSpeed = 250.0f;
}

// Exit aiming state: restore defaults.
void ATPSCharacter::StopAim()
{
    bIsAiming = false;
    SpringArm->TargetArmLength = 400.0f;
    Camera->FieldOfView = DefaultFOV;
    GetCharacterMovement()->MaxWalkSpeed = 500.0f;
}

// Enter cover: reduce walk speed.
void ATPSCharacter::StartCover()
{
    if (!bIsInCover) { bIsInCover = true; GetCharacterMovement()->MaxWalkSpeed = 150.0f; }
}

// Exit cover: restore walk speed.
void ATPSCharacter::StopCover()
{
    if (bIsInCover) { bIsInCover = false; GetCharacterMovement()->MaxWalkSpeed = 500.0f; }
}

// Switch weapon based on scroll input.
void ATPSCharacter::SwitchWeapon(const FInputActionValue& Value)
{
    float Scroll = Value.Get<float>();
    if (Scroll != 0.0f && CurrentWeapon) CurrentWeapon->Destroy();
}

// Server RPC: spawn default weapon and attach to gun mesh.
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

// Apply damage and destroy if health reaches zero.
float ATPSCharacter::TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer)
{
    float Actual = Super::TakeDamage(Damage, Event, Instigator, Causer);
    CurrentHealth = FMath::Clamp(CurrentHealth - Actual, 0.0f, MaxHealth);
    if (CurrentHealth <= 0.0f && HasAuthority()) Destroy();
    return Actual;
}

// Replicate health and current weapon.
void ATPSCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(ATPSCharacter, CurrentHealth);
    DOREPLIFETIME(ATPSCharacter, CurrentWeapon);
}
