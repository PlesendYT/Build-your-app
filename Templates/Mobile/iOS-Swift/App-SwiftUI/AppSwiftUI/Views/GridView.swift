import SwiftUI

/// Displays a scrollable grid of numbered tiles using `LazyVGrid`.
struct GridView: View {
    /// Adaptive grid columns that adjust between 80 and 120 points wide.
    let columns = [GridItem(.adaptive(minimum: 80, maximum: 120))]

    var body: some View {
        NavigationStack {
            ScrollView {
                // Lazy vertical grid that renders items on demand.
                LazyVGrid(columns: columns, spacing: 16) {
                    ForEach(1...50, id: \.self) { index in
                        RoundedRectangle(cornerRadius: 12)
                            .fill(Color.indigo.opacity(0.2))
                            .frame(height: 80)
                            .overlay(Text("\(index)").foregroundColor(.indigo))
                    }
                }
                .padding()
            }
            .navigationTitle("Grid")
        }
    }
}

#Preview {
    GridView()
}
