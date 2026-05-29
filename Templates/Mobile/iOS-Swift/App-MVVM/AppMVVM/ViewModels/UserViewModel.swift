import Foundation
import Combine

/// View model that manages the state for the user list.
/// Marked with `@MainActor` so all published properties update on the main thread.
@MainActor
class UserViewModel: ObservableObject {
    /// The list of users fetched from the API.
    @Published var users: [User] = []
    /// Indicates whether a fetch operation is in progress.
    @Published var isLoading = false
    /// Holds an error message if the fetch fails.
    @Published var errorMessage: String?

    /// Shared API service instance used to perform network requests.
    private let apiService = APIService.shared

    /// Loads users from the API asynchronously.
    /// Updates `users` on success or `errorMessage` on failure.
    func loadUsers() async {
        isLoading = true
        errorMessage = nil
        do {
            users = try await apiService.fetchUsers()
        } catch {
            errorMessage = error.localizedDescription
        }
        isLoading = false
    }
}
