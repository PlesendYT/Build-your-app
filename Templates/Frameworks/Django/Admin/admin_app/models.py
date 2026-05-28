from django.contrib import admin
from django.db import models

class DashboardMetric(models.Model):
    label = models.CharField(max_length=100)
    value = models.CharField(max_length=100)
    change = models.CharField(max_length=20, blank=True)
    is_positive = models.BooleanField(default=True)

    def __str__(self):
        return self.label

@admin.register(DashboardMetric)
class DashboardMetricAdmin(admin.ModelAdmin):
    list_display = ('label', 'value', 'change', 'is_positive')
    list_editable = ('value', 'change', 'is_positive')
