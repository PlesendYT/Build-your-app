import SwiftUI

/// The root view for the MVVM template.
/// Displays a list of users fetched from the API, with loading and error states.
struct ContentView: View {
    /// The view model that drives the data and state for this view.
    @StateObject private var viewModel = UserViewModel()

    var body: some View {
        NavigationStack {
            // Renders each user as a row with name and email.
            List(viewModel.users) { user in
                VStack(alignment: .leading) {
                    Text(user.name).font(.headline)
                    Text(user.email).font(.subheadline).foregroundColor(.secondary)
                }
            }
            .navigationTitle("Users")
            // Triggers the data load when the view appears.
            .task { await viewModel.loadUsers() }
            // Shows a spinner while loading or an error message if something went wrong.
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
