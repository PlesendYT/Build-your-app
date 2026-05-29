import SwiftUI

/// The main view for the Basic App template.
/// Displays a greeting, a tap counter, and a button that increments the count.
struct ContentView: View {
    /// Tracks how many times the user has tapped the button.
    @State private var tapCount = 0

    var body: some View {
        // Vertical stack with spacing for the greeting, counter, and button.
        VStack(spacing: 20) {
            Text("Hello, Build Your App!")
                .font(.largeTitle)
                .foregroundColor(.blue)

            // Displays the current number of taps.
            Text("You tapped \(tapCount) times")
                .font(.title2)

            // Button that increments the tap counter.
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
