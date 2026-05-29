// Root layout — sets the HTML shell with metadata and global body styles
import type { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'Next.js App',
  description: 'Next.js TypeScript Template',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="de">
      <body style={{ fontFamily: 'system-ui, sans-serif', margin: 0, padding: '1rem', background: '#f5f5f5' }}>
        {children}
      </body>
    </html>
  );
}
