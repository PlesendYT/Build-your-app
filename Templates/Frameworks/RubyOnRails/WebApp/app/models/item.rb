# Item model with validations for title and description
class Item < ApplicationRecord
  validates :title, presence: true, length: { maximum: 200 }
  validates :description, presence: true
end
