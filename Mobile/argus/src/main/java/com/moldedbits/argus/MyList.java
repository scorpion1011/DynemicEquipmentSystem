package com.moldedbits.argus;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class MyList {
    @SerializedName("idList")
    @Expose
    private int idList;

    @SerializedName("name")
    @Expose
    private String listName;

    public MyList (int _idList, String _listName) {
        idList = _idList;
        listName = _listName;
    }

    public int getIdList() {
        return idList;
    }

    public void setIdList(int idList) {
        this.idList = idList;
    }

    public String getListName() {
        return listName;
    }

    public void getListName(String listName) {
        this.listName = listName;
    }
}
