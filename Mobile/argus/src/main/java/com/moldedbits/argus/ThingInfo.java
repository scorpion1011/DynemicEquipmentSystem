package com.moldedbits.argus;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ThingInfo {
    @SerializedName("idThing")
    @Expose
    private int id;

    public int getIdThing() {
        return id;
    }

    public void setIdThing(int id) {
        this.id = id;
    }

    @SerializedName("name")
    @Expose
    private String name;

    public String getThingName() {
        return name;
    }

    public void setThingName(String name) {
        this.name = name;
    }

    @SerializedName("isActive")
    @Expose
    private Boolean presence;

    public Boolean getPresence() {
        return presence;
    }

    public void setPresence(Boolean presence) {
        this.presence = presence;
    }
}
