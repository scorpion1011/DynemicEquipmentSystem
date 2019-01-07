package com.moldedbits.argus;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;

import java.util.ArrayList;
import java.util.List;

public class MyListsActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    Thread myCurrentThread = new Thread();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //setContentView(R.layout.content_my_lists);
        setContentView(R.layout.activity_my_lists);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

//        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
//        fab.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
//                        .setAction("Action", null).show();
//            }
//        });



        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
        Menu menu = navigationView.getMenu();
        menu.clear();

        for (MyList list:getLists()) {
            menu.add(Menu.NONE, list.id, Menu.NONE, list.name);
        }

        onNavigationItemSelected(menu.getItem(0));


    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.my_lists, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();



        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(final MenuItem item) {
        stopThread();
        final RecyclerView recList = (RecyclerView) findViewById(R.id.thingsList);
        recList.setHasFixedSize(true);
        LinearLayoutManager llm = new LinearLayoutManager(this);
        llm.setOrientation(LinearLayoutManager.VERTICAL);
        recList.setLayoutManager(llm);

        //ThingAdapter ca = new ThingAdapter(createList((int) (Math.random()*8 + 1), item.getTitle()));
        //recList.setAdapter(ca);
//        Timer mTimer = new Timer();
//        MyTimerTask mMyTimerTask = new MyTimerTask(recList, item);
//        mTimer.schedule(mMyTimerTask, 1000, 5000);

        myCurrentThread = new Thread() {
            public void run() {
                int i = 0;
                while (!Thread.currentThread().isInterrupted()) {
                    try {
                        runOnUiThread(new Runnable() {

                            @Override
                            public void run() {
                                ThingAdapter ca = new ThingAdapter(createList((int) (Math.random() * 8 + 1), item.getTitle()));
                                recList.setAdapter(ca);
                            }
                        });
                        Thread.sleep(1000);
                    } catch (InterruptedException e) {
                        break;
                    }
                }
            }
        };

        myCurrentThread.start();//runThread(recList, item);

        // Handle navigation view item clicks here.
        int id = item.getItemId();

        setTitle(getResources().getText(R.string.title_activity_my_lists).toString() + ": " + item.getTitle());

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    public void LogOutOnClick(MenuItem item) {
        Argus.getInstance().setState(ArgusState.SIGNED_OUT);
        Intent intent = new Intent(this, ArgusActivity.class);
        startActivity(intent);
    }

    protected List<MyList> getLists()
    {
        List<MyList> lists = new ArrayList<MyList>();

        lists.add(new MyList(123, "List1"));
        lists.add(new MyList(124, "List2"));
        lists.add(new MyList(125, "List3"));

        return lists;
    }

    private void runThread(final RecyclerView recList, final MenuItem item) {
        new Thread() {
            public void run() {
                int i = 0;
                while (!isInterrupted()) {
                    try {
                        runOnUiThread(new Runnable() {

                            @Override
                            public void run() {
                                ThingAdapter ca = new ThingAdapter(createList((int) (Math.random() * 8 + 1), item.getTitle()));
                                recList.setAdapter(ca);
                            }
                        });
                        Thread.sleep(1000);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            }
        }.start();
    }

    private List<ThingInfo> createList(int size, CharSequence title) {

        List<ThingInfo> result = new ArrayList<ThingInfo>();
        for (int i=1; i <= size; i++) {
            ThingInfo ci = new ThingInfo();
            ci.name = "ThingInfo #" + i + " " + title;
            ci.presence = ((int) (Math.random()*2 + 1)) % 2 == 0;
            ci.id = i + 1;

            result.add(ci);
        }

        return result;
    }

    public void stopThread() {
        if(myCurrentThread != null){
            myCurrentThread.interrupt();
        }
    }
}

//class MyTimerTask extends TimerTask {
//    RecyclerView _recList;
//    MenuItem _item;
//
//    public MyTimerTask(RecyclerView recList, MenuItem item) {
//        _recList = recList;
//        _item = item;
//    }
//
//    @Override
//    public void run() {
//        MyListsActivity.this.runOnUiThread(new Runnable() {
//            public void run() {
//                ThingAdapter ca = new ThingAdapter(createList((int) (Math.random() * 8 + 1), _item.getTitle()));
//                _recList.setAdapter(ca);
//            }
//        });
//    }
//
//    private List<ThingInfo> createList(int size, CharSequence title) {
//
//        List<ThingInfo> result = new ArrayList<ThingInfo>();
//        for (int i=1; i <= size; i++) {
//            ThingInfo ci = new ThingInfo();
//            ci.name = "ThingInfo #" + i + " " + title;
//            ci.presence = ((int) (Math.random()*2 + 1)) % 2 == 0;
//            ci.id = i + 1;
//
//            result.add(ci);
//        }
//
//        return result;
//    }
//}
