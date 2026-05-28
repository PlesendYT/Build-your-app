<?php

namespace App\Http\Controllers\Api;

use App\Models\Item;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;

class ItemController extends Controller
{
    public function index() {
        return Item::all();
    }

    public function store(Request $request) {
        $validated = $request->validate(['title' => 'required', 'description' => 'required']);
        return Item::create($validated);
    }

    public function show(Item $item) {
        return $item;
    }

    public function destroy(Item $item) {
        $item->delete();
        return response()->noContent();
    }
}
