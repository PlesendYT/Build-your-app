plugins {
    id("com.android.application")
    kotlin("android")
}
android {
    namespace = "example.demo"
    compileSdk = 34
    defaultConfig { applicationId = "example.demo"; minSdk = 24; targetSdk = 34; versionCode = 1; versionName = "0.1.0" }
    buildFeatures { compose = true }
    composeOptions { kotlinCompilerExtensionVersion = "1.5.10" }
    kotlinOptions { jvmTarget = "17" }
}
dependencies {
    implementation("androidx.core:core-ktx:1.13.0")
    implementation("androidx.activity:activity-compose:1.9.0")
    implementation(platform("androidx.compose:compose-bom:2024.02.00"))
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.material3:material3")
}