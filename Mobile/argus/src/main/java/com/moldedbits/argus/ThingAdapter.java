package com.moldedbits.argus;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.List;

public class ThingAdapter extends RecyclerView.Adapter<ThingAdapter.ThingViewHolder> {

    private List<ThingInfo> contactList;

    public ThingAdapter(List<ThingInfo> contactList) {
        this.contactList = contactList;
    }


    @Override
    public int getItemCount() {
        return contactList.size();
    }

    @Override
    public void onBindViewHolder(ThingViewHolder thingViewHolder, int i) {
        ThingInfo ci = contactList.get(i);
        thingViewHolder.vName.setText(ci.name);
        if(ci.presence) {
            thingViewHolder.vPresenceON.setVisibility(View.VISIBLE);
            thingViewHolder.vPresenceOFF.setVisibility(View.INVISIBLE);
        }
        else {
            thingViewHolder.vPresenceOFF.setVisibility(View.VISIBLE);
            thingViewHolder.vPresenceON.setVisibility(View.INVISIBLE);
        }
    }

    @Override
    public ThingViewHolder onCreateViewHolder(ViewGroup viewGroup, int i) {
        View itemView = LayoutInflater.
                from(viewGroup.getContext()).
                inflate(R.layout.card_layout, viewGroup, false);

        return new ThingViewHolder(itemView);
    }

    public static class ThingViewHolder extends RecyclerView.ViewHolder {

        protected TextView vName;
        protected ImageView vPresenceON;
        protected ImageView vPresenceOFF;

        public ThingViewHolder(View v) {
            super(v);
            vName =  (TextView) v.findViewById(R.id.name);
            vPresenceON =  (ImageView) v.findViewById(R.id.presenceON);
            vPresenceOFF =  (ImageView) v.findViewById(R.id.presenceOFF);
        }
    }
}