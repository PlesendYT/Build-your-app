import SwiftUI

struct GridView: View {
    let columns = [GridItem(.adaptive(minimum: 80, maximum: 120))]

    var body: some View {
        NavigationStack {
            ScrollView {
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
