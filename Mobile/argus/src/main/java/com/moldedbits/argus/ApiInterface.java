package com.moldedbits.argus;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface ApiInterface {
    @Headers("Content-Type:application/json")
    @GET("user/{email}/{password}")
    Call<User> getUser(
            @Path("email") String email,
            @Path("password") String password
    );
}
