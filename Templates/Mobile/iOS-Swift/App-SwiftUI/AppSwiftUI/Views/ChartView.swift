import SwiftUI
import Charts

/// Represents a single month's sales data point for the chart.
struct MonthlySales: Identifiable {
    /// The abbreviated month name (e.g. "Jan").
    let month: String
    /// The sales amount for that month.
    let sales: Double
    /// Conforms to `Identifiable` using the month string as the unique ID.
    var id: String { month }
}

/// Displays a bar chart of monthly sales data using the Swift Charts framework.
struct ChartView: View {
    /// Hardcoded sample data for six months.
    let data: [MonthlySales] = [
        .init(month: "Jan", sales: 120), .init(month: "Feb", sales: 190),
        .init(month: "Mar", sales: 160), .init(month: "Apr", sales: 210),
        .init(month: "May", sales: 180), .init(month: "Jun", sales: 240)
    ]

    var body: some View {
        NavigationStack {
            // Renders a bar chart with month on the x-axis and sales on the y-axis.
            Chart(data) { item in
                BarMark(
                    x: .value("Month", item.month),
                    y: .value("Sales", item.sales)
                )
                .foregroundStyle(.blue.gradient)
            }
            .chartXAxisLabel("Month")
            .chartYAxisLabel("Sales")
            .frame(height: 300)
            .padding()
            .navigationTitle("Charts")
        }
    }
}

#Preview {
    ChartView()
}
