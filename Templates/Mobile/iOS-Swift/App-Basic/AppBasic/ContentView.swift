import SwiftUI

struct ContentView: View {
    @State private var tapCount = 0

    var body: some View {
        VStack(spacing: 20) {
            Text("Hello, Build Your App!")
                .font(.largeTitle)
                .foregroundColor(.blue)

            Text("You tapped \(tapCount) times")
                .font(.title2)

            Button("Tap me") {
                tapCount += 1
            }
            .buttonStyle(.borderedProminent)
        }
        .padding()
    }
}

#Preview {
    ContentView()
}
