package com.buildyourapp.mvvm.data.db

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import com.buildyourapp.mvvm.data.model.Item
import com.buildyourapp.mvvm.data.dao.ItemDao

/** Room database class for the MVVM app. Holds the single Item entity and exposes its DAO. */
@Database(entities = [Item::class], version = 1)
abstract class AppDatabase : RoomDatabase() {
    /** Returns the DAO for item CRUD operations. */
    abstract fun itemDao(): ItemDao

    companion object {
        @Volatile
        private var INSTANCE: AppDatabase? = null

        /** Returns the singleton AppDatabase instance, creating it if needed (thread-safe). */
        fun getInstance(context: Context): AppDatabase {
            return INSTANCE ?: synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    AppDatabase::class.java,
                    "app_database"
                ).build()
                INSTANCE = instance
                instance
            }
        }
    }
}
