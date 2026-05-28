plugins {
    id("com.android.application")
    kotlin("android")
    id("org.jetbrains.compose")
}

android {
    namespace = "com.example.mpandroid"
    compileSdk = 34
    defaultConfig {
        applicationId = "com.example.mpandroid"
        minSdk = 26; targetSdk = 34
    }
    buildFeatures { compose = true }
    composeOptions { kotlinCompilerExtensionVersion = "1.5.6" }
    kotlinOptions { jvmTarget = "21" }
}

dependencies {
    implementation(project(":shared"))
    implementation(compose.material3)
    implementation("androidx.activity:activity-compose:1.8.2")
}
