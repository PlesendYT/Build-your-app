// Kotlin buildscript for a Paper plugin using Kotlin
plugins {
    kotlin("jvm") version "2.0.21"
    id("io.papermc.paperweight.userdev") version "2.0.0-beta.14"
}

group = "com.example"
version = "1.0.0"
base.archivesName.set("KotlinPlugin")

java {
    toolchain.languageVersion.set(JavaLanguageVersion.of(21))
}

kotlin {
    jvmToolchain(21)
}

repositories {
    mavenCentral()
    // PaperMC repository for the Paper API
    maven("https://repo.papermc.io/repository/maven-public/")
}

dependencies {
    // Paper API — provided by the server runtime
    paperweight.paperApi("1.21.4-R0.1-SNAPSHOT")

    // Kotlin standard library (bundled by Paper)
    implementation(kotlin("stdlib"))
}

tasks.processResources {
    // Substitute tokens in plugin.yml if needed
    filteringCharset = "UTF-8"
}
