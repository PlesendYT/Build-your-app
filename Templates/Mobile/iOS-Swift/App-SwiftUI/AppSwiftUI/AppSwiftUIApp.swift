import SwiftUI

/// The entry point for the SwiftUI App template.
/// Hosts a `TabView` with Home, Grid, and Charts tabs.
@main
struct AppSwiftUIApp: App {
    /// Tracks the currently selected tab index.
    @State private var selectedTab = 0

    var body: some Scene {
        WindowGroup {
            // Tab-based navigation with three primary sections.
            TabView(selection: $selectedTab) {
                HomeView()
                    .tabItem { Label("Home", systemImage: "house") }
                    .tag(0)

                GridView()
                    .tabItem { Label("Grid", systemImage: "square.grid.2x2") }
                    .tag(1)

                ChartView()
                    .tabItem { Label("Charts", systemImage: "chart.bar") }
                    .tag(2)
            }
        }
    }
}
