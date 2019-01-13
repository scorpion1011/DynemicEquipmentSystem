//package com.moldedbits.argus;
//
//import android.view.Menu;
//
//import java.util.List;
//
//import retrofit2.Call;
//import retrofit2.Callback;
//import retrofit2.Response;
//
//public class MyCallback implements Callback<T> {
//    Menu _menu;
//    public MyCallback (Menu menu) {
//        _menu = menu;
//    }
//
//    @Override
//    public void onResponse(Call<List<MyList>> call, Response<List<MyList>> response) {
//
//        List<MyList> checkLists = response.body();
//
//        if (checkLists != null) {
//            for (final MyList list : checkLists) {
//                lists.add(new MyList(list.getIdList(), list.getListName()));
//            }
//        }
//    }
//
//    @Override
//    public void onFailure(Call<List<MyList>> call, Throwable t) {
//
//    }
//}
