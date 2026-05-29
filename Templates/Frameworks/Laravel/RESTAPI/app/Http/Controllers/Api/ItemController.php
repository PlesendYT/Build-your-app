<?php

// API controller for managing Item resources via REST endpoints
namespace App\Http\Controllers\Api;

use App\Models\Item;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;

class ItemController extends Controller
{
    // List all items
    public function index() {
        return Item::all();
    }

    // Create a new item with validated data
    public function store(Request $request) {
        $validated = $request->validate(['title' => 'required', 'description' => 'required']);
        return Item::create($validated);
    }

    // Show a single item by ID
    public function show(Item $item) {
        return $item;
    }

    // Delete an item and return 204 No Content
    public function destroy(Item $item) {
        $item->delete();
        return response()->noContent();
    }
}
