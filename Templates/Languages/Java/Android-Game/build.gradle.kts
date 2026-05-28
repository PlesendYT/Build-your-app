plugins {
    id("com.android.application")
}

android {
    namespace = "com.example.androidgame"
    compileSdk = 34

    defaultConfig {
        applicationId = "com.example.androidgame"
        minSdk = 26
        targetSdk = 34
        versionCode = 1
        versionName = "1.0"
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_21
        targetCompatibility = JavaVersion.VERSION_21
    }
}

dependencies {
    implementation("androidx.appcompat:appcompat:1.6.1")
}
