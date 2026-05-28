from rest_framework import serializers

class ItemSerializer(serializers.Serializer):
    id = serializers.IntegerField(read_only=True)
    title = serializers.CharField(max_length=200)
    description = serializers.CharField()
    created_at = serializers.DateTimeField(read_only=True)
