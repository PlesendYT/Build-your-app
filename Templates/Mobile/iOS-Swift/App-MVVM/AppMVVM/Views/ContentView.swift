import SwiftUI

struct ContentView: View {
    @StateObject private var viewModel = UserViewModel()

    var body: some View {
        NavigationStack {
            List(viewModel.users) { user in
                VStack(alignment: .leading) {
                    Text(user.name).font(.headline)
                    Text(user.email).font(.subheadline).foregroundColor(.secondary)
                }
            }
            .navigationTitle("Users")
            .task { await viewModel.loadUsers() }
            .overlay {
                if viewModel.isLoading { ProgressView() }
                if let error = viewModel.errorMessage {
                    Text(error).foregroundColor(.red)
                }
            }
        }
    }
}

#Preview {
    ContentView()
}
