import type { Metadata } from "next";

// Root layout metadata for the AI chat application
export const metadata: Metadata = {
  title: "AI Chat",
  description: "Next.js AI Chat Interface",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // Render the HTML document with a minimal dark-themed body
  return (
    <html lang="en">
      <body style={{ margin: 0, background: "#0f0f0f", color: "#e0e0e0", fontFamily: "system-ui, sans-serif" }}>
        {children}
      </body>
    </html>
  );
}
