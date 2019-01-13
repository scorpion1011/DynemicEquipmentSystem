package com.moldedbits.argus.samples.email_social_login;

import android.widget.Toast;

import com.moldedbits.argus.ApiClient;
import com.moldedbits.argus.ApiInterface;
import com.moldedbits.argus.Argus;
import com.moldedbits.argus.ArgusState;
import com.moldedbits.argus.User;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by abhishek
 * on 16/06/17.
 *
 * This is a sample implementation of default login provider available in Argus
 */

public class SimpleEmailLoginProvider extends com.moldedbits.argus.provider.login.EmailLoginProvider {

    /**
     * this function is called after user input was validated
     * this is where actual API call to your server will go
     */

    private ApiInterface apiInterface;
    private User user;

    @Override
    protected void doServerLogin(String username, String password) {
        // need to set state signed-in in Argus here
        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);

            Call<User> call = apiInterface.getUser(username, password);//"alex.kom@gmail.com", "12345");

        call.enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {

                user = response.body();

                String id = "There is no such user";

                if (user != null) {
                    id = user.getIdUser();
                }

                if (id.equals("There is no such user")) {
                    if (context != null) {
                        Toast.makeText(context, context.getString(R.string.invalid_email),
                                Toast.LENGTH_LONG).show();
                    }
                }
                else {
                    if (resultListener != null) {
                        resultListener.onSuccess(ArgusState.SIGNED_IN, id);
                    }
                }
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                String message = t.getMessage();
            }
        });

//        if(true || username.equals("valid@user.com") && password.equals("password")) {
//            // do a real API call here and in on success do following
//            if (resultListener != null) {
//                resultListener.onSuccess(ArgusState.SIGNED_IN);
//            }
//        } else {
//            if (context != null) {
//                Toast.makeText(context, context.getString(R.string.invalid_email),
//                        Toast.LENGTH_LONG).show();
//            }
//        }
    }
}
