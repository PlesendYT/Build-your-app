import SwiftUI
import Charts

struct MonthlySales: Identifiable {
    let month: String
    let sales: Double
    var id: String { month }
}

struct ChartView: View {
    let data: [MonthlySales] = [
        .init(month: "Jan", sales: 120), .init(month: "Feb", sales: 190),
        .init(month: "Mar", sales: 160), .init(month: "Apr", sales: 210),
        .init(month: "May", sales: 180), .init(month: "Jun", sales: 240)
    ]

    var body: some View {
        NavigationStack {
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
