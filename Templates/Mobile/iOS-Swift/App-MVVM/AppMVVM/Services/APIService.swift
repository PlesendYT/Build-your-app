import Foundation

/// Errors that can occur during API requests.
enum APIError: Error {
    /// The constructed URL is invalid.
    case invalidURL
    /// The network request failed.
    case requestFailed
    /// The response data could not be decoded.
    case decodingFailed
}

/// Provides a shared networking layer for fetching data from a remote API.
class APIService {
    /// Shared singleton instance.
    static let shared = APIService()
    /// Base URL for the JSONPlaceholder API.
    private let baseURL = "https://jsonplaceholder.typicode.com"

    /// Fetches the list of users from the API.
    /// - Returns: An array of `User` objects.
    /// - Throws: An `APIError` if the URL is invalid or decoding fails.
    func fetchUsers() async throws -> [User] {
        // Build the full URL for the users endpoint.
        guard let url = URL(string: "\(baseURL)/users") else {
            throw APIError.invalidURL
        }
        // Perform the network request.
        let (data, _) = try await URLSession.shared.data(from: url)
        // Decode the JSON response into an array of User objects.
        guard let users = try? JSONDecoder().decode([User].self, from: data) else {
            throw APIError.decodingFailed
        }
        return users
    }
}
