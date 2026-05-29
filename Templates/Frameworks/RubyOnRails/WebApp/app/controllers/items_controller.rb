# Controller for managing Item resources in the web app
class ItemsController < ApplicationController
  # List all items
  def index
    @items = Item.all
  end

  # Show a single item by ID
  def show
    @item = Item.find(params[:id])
  end

  # Render the new item form
  def new
    @item = Item.new
  end

  # Create a new item with validated parameters
  def create
    @item = Item.new(item_params)
    if @item.save
      redirect_to @item, notice: "Item erstellt."
    else
      render :new, status: :unprocessable_entity
    end
  end

  private

  # Strong parameters for item creation
  def item_params
    params.require(:item).permit(:title, :description)
  end
end
