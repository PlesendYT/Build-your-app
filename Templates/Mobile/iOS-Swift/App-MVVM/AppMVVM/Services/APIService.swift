import Foundation

enum APIError: Error {
    case invalidURL
    case requestFailed
    case decodingFailed
}

class APIService {
    static let shared = APIService()
    private let baseURL = "https://jsonplaceholder.typicode.com"

    func fetchUsers() async throws -> [User] {
        guard let url = URL(string: "\(baseURL)/users") else {
            throw APIError.invalidURL
        }
        let (data, _) = try await URLSession.shared.data(from: url)
        guard let users = try? JSONDecoder().decode([User].self, from: data) else {
            throw APIError.decodingFailed
        }
        return users
    }
}
