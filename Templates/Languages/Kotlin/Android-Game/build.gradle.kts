plugins {
    id("com.android.application")
    kotlin("android")
}

android {
    namespace = "com.example.kotlingame"
    compileSdk = 34

    defaultConfig {
        applicationId = "com.example.kotlingame"
        minSdk = 26
        targetSdk = 34
        versionCode = 1
        versionName = "1.0"
    }

    kotlinOptions { jvmTarget = "21" }
}

dependencies {
    implementation("androidx.appcompat:appcompat:1.6.1")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.7.3")
}
