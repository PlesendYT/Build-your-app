import SwiftUI

@main
struct AppSwiftUIApp: App {
    @State private var selectedTab = 0

    var body: some Scene {
        WindowGroup {
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
