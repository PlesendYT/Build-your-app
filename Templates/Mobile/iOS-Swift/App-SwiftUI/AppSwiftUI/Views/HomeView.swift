import SwiftUI

struct HomeView: View {
    @State private var items = Array(1...20)
    @Namespace private var animation

    var body: some View {
        NavigationStack {
            List {
                ForEach(items, id: \.self) { item in
                    NavigationLink(destination: DetailView(item: item, animation: animation)) {
                        HStack {
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

struct DetailView: View {
    let item: Int
    var animation: Namespace.ID

    var body: some View {
        VStack {
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
