{{-- Home page view displaying hero section and items grid --}}
@extends('layouts.app')
@section('title', 'Home')
@section('content')
<div class="hero">
    <h2>Willkommen bei LaravelApp</h2>
    <p>Moderne Laravel-Anwendung mit Blade</p>
    <a href="{{ route('about') }}" class="btn">Mehr erfahren</a>
</div>
<section>
    <h3>Daten</h3>
    <div class="data-grid">
        @foreach($items as $item)
        <div class="data-card">
            <h4>{{ $item->title }}</h4>
            <p>{{ $item->description }}</p>
        </div>
        @endforeach
    </div>
</section>
@endsection
