{{-- Main application layout with header, navigation, and footer --}}
<!DOCTYPE html>
<html lang="de">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@yield('title', 'LaravelApp')</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: system-ui, sans-serif; background: #f8f9fa; color: #333; }
        .app-layout { display: flex; flex-direction: column; min-height: 100vh; }
        .header { background: #fff; border-bottom: 1px solid #dee2e6; padding: 1rem 2rem; display: flex; align-items: center; justify-content: space-between; }
        .logo { font-size: 1.5rem; color: #f9322c; }
        .nav a { text-decoration: none; color: #333; margin-left: 1.5rem; }
        .main { flex: 1; padding: 2rem; max-width: 1200px; margin: 0 auto; width: 100%; }
        .footer { background: #fff; border-top: 1px solid #dee2e6; padding: 1rem; text-align: center; }
        .hero { text-align: center; padding: 4rem 0; }
        .btn { display: inline-block; background: #f9322c; color: #fff; padding: 0.6rem 1.5rem; border-radius: 8px; text-decoration: none; }
        .data-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; margin-top: 1.5rem; }
        .data-card { background: #fff; border: 1px solid #dee2e6; border-radius: 8px; padding: 1.5rem; }
    </style>
</head>
<body>
    <div class="app-layout">
        <header class="header">
            <h1 class="logo">LaravelApp</h1>
            <nav class="nav">
                <a href="{{ route('home') }}">Home</a>
                <a href="{{ route('about') }}">About</a>
                <a href="{{ route('contact') }}">Contact</a>
            </nav>
        </header>
        <main class="main">@yield('content')</main>
        <footer class="footer"><p>&copy; @php echo date('Y') @endphp LaravelApp</p></footer>
    </div>
</body>
</html>
