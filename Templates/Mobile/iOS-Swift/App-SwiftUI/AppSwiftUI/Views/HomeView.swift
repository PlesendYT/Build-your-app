import SwiftUI

/// The main list view that displays items and navigates to a detail screen.
/// Demonstrates `matchedGeometryEffect` for smooth animated transitions.
struct HomeView: View {
    /// Array of item identifiers displayed in the list.
    @State private var items = Array(1...20)
    /// Namespace shared with `DetailView` for matched geometry transitions.
    @Namespace private var animation

    var body: some View {
        NavigationStack {
            List {
                ForEach(items, id: \.self) { item in
                    // Each row navigates to a detail view with the shared animation namespace.
                    NavigationLink(destination: DetailView(item: item, animation: animation)) {
                        HStack {
                            // Colored circle that participates in the geometry transition.
                            Circle()
                                .fill(Color.blue.opacity(0.3))
                                .frame(width: 40, height: 40)
                                .overlay(Text("\(item)").font(.caption))
                                .matchedGeometryEffect(id: item, in: animation)

                            Text("Item \(item)")
                        }
                    }
                }
            }
            .navigationTitle("Home")
        }
    }
}

/// The detail view for a selected item, with a larger circle and matched geometry effect.
struct DetailView: View {
    /// The item identifier to display.
    let item: Int
    /// The shared namespace from `HomeView` for the animated transition.
    var animation: Namespace.ID

    var body: some View {
        VStack {
            // Larger circle that matches the geometry of the list item's circle.
            Circle()
                .fill(Color.blue.opacity(0.3))
                .frame(width: 100, height: 100)
                .overlay(Text("\(item)").font(.title))
                .matchedGeometryEffect(id: item, in: animation)

            Text("Detail for Item \(item)")
                .font(.title2)
        }
        .navigationTitle("Detail")
    }
}

#Preview {
    HomeView()
}
