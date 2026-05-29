import Foundation

/// Represents a user fetched from a remote API.
/// Conforms to `Codable` for JSON parsing and `Identifiable` for SwiftUI lists.
struct User: Codable, Identifiable {
    /// The unique identifier of the user.
    let id: Int
    /// The full name of the user.
    let name: String
    /// The email address of the user.
    let email: String
}
