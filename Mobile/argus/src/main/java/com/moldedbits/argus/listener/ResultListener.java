package com.moldedbits.argus.listener;

import com.moldedbits.argus.ArgusState;

public interface ResultListener {
    void onSuccess(ArgusState argusState);

    void onSuccess(ArgusState argusState, String userId);

    void onFailure(String message);
}
