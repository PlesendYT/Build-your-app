package android.app;

import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import androidx.appcompat.app.AppCompatActivity;
import com.example.androidapp.R;
import com.example.androidapp.databinding.ActivityMainBinding;

import java.util.ArrayList;
import java.util.stream.Stream;

public final class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;
    private final ArrayList<String> items = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        Stream.of("Android", "Java", "Kotlin", "Compose")
            .forEach(items::add);

        var adapter = new ArrayAdapter<>(this,
            android.R.layout.simple_list_item_1, items);
        binding.listView.setAdapter(adapter);

        binding.fabAdd.setOnClickListener(v -> {
            items.add("Neues Item #" + (items.size() + 1));
            adapter.notifyDataSetChanged();
        });
    }
}
