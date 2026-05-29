/**
 * Android App sample: a basic activity with a ListView and a FAB to add items.
 */
package android.app;

import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import androidx.appcompat.app.AppCompatActivity;
import com.example.androidapp.R;
import com.example.androidapp.databinding.ActivityMainBinding;

import java.util.ArrayList;
import java.util.stream.Stream;

/** Main activity displaying a dynamic list with a floating-action button. */
public final class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;
    private final ArrayList<String> items = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        // Populate with initial preset items
        Stream.of("Android", "Java", "Kotlin", "Compose")
            .forEach(items::add);

        // Wire up the ListView adapter
        var adapter = new ArrayAdapter<>(this,
            android.R.layout.simple_list_item_1, items);
        binding.listView.setAdapter(adapter);

        // FAB appends a new item to the list
        binding.fabAdd.setOnClickListener(v -> {
            items.add("Neues Item #" + (items.size() + 1));
            adapter.notifyDataSetChanged();
        });
    }
}
