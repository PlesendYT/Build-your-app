<?php

// Web controller for item management
namespace App\Http\Controllers;

use App\Models\Item;
use Illuminate\Http\Request;

class ItemController extends Controller
{
    // Display all items on the home page
    public function index()
    {
        $items = Item::all();
        return view('home', compact('items'));
    }

    // Store a new item from the contact form
    public function store(Request $request)
    {
        $validated = $request->validate([
            'title' => 'required|max:200',
            'description' => 'required',
        ]);

        Item::create($validated);
        return back()->with('success', 'Item erstellt!');
    }
}
