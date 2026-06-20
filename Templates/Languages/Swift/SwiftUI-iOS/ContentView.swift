import SwiftUI

struct ContentView: View {
    @State private var count = 0
    var body: some View {
        VStack(spacing: 12) {
            Text("SwiftUI Template").font(.title)
            Text("Count: \(count)")
            Button("Increment") { count += 1 }
        }
        .padding()
    }
}