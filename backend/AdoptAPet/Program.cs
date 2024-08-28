using AdoptAPet.Data;
using AdoptAPet.Models;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
   // .AddDefaultUI();

   builder.Services.ConfigureApplicationCookie(options =>
   {
       options.Cookie.HttpOnly = true;
       options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
       options.LoginPath = "/api/auth/login";
       options.SlidingExpiration = true;
   });

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

AddBasicActors();
AddBasicPetsAndAdvetrisements();

app.Run();

void AddBasicActors()
{
    var tAdmin = CreateAdminIfNotExists();
    tAdmin.Wait();

    var tRescueTeamMember = CreateRescueTeamMemberIfNotExists();
    tRescueTeamMember.Wait();

    var tRegularUser = CreateRegularUserIfNotExists();
    tRegularUser.Wait();
}

async Task CreateAdminIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var adminInDb = await userManager.FindByEmailAsync("admin@adoptapet.com");
    if (adminInDb == null)
    {
        var admin = new User
        {
            FirstName = "Administrator",
            LastName = "of the Application",
            Email = "admin@adoptapet.com",
            UserName = "admin@adoptapet.com"
        };
        var adminCreated = await userManager.CreateAsync(admin, "String!0");

        if (adminCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

async Task CreateRescueTeamMemberIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var rescueTeamMemberInDb = await userManager.FindByEmailAsync("staff@adoptapet.com");
    if (rescueTeamMemberInDb == null)
    {
        var rescueTeamMember = new User
        {
            FirstName = "Francis",
            LastName = "of Assisi",
            Email = "staff@adoptapet.com",
            UserName = "staff@adoptapet.com"
        };
        var rescueTeamMemberCreated = await userManager.CreateAsync(rescueTeamMember, "String!1");

        if (rescueTeamMemberCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(rescueTeamMember, "Rescue Team");
        }
    }
}

async Task CreateRegularUserIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var regularUserInDb = await userManager.FindByEmailAsync("tom.taylor@email.com");
    if (regularUserInDb == null)
    {
        var regularUser = new User
        {
            FirstName = "Tom",
            LastName = "Taylor",
            Email = "tom.taylor@email.com",
            UserName = "tom.taylor@email.com"
        };
        var regularUserCreated = await userManager.CreateAsync(regularUser, "String!2");

        if (regularUserCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(regularUser, "User");
        }
    }
}

void AddBasicPetsAndAdvetrisements()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    if (!dbContext.Pets.Any())
    {
        var pet1 = new Pet
        {
            Name = "Buddy",
            Species = Species.Dog,
            Birth = new DateTime(2020, 1, 1).ToUniversalTime(),
            Gender = Gender.Male,
            IsNeutered = true,
            Description = "Friendly dog looking for a loving home.",
            PictureLink = "https://lh3.googleusercontent.com/fife/ALs6j_FlqhtpgXlUJ8J-bljpE5jbRM67Jx6p75so4UcbOHq90kXI8zCfvv3-VB5a_Dq0fz6ON3CMTM1-3ft6D8T0ny_c9yU3mW4GIfDuLNXs2VW79sXjgSPRdQ9jxKrM8QT19W7ZaaWnXug8HsrHT6uGc9LeXCHMg-PjFXKLOWQ0BBssV-KjRG94VckRv9nGcXeX8ELQkOWOPGc8gy4dAytGtAzApx-Y7vOKy1CRd7p8T45UWmfC4UZ_8wtWUsfzUALBogbQQjoQNvaaDvXytXJUH8smcP8vALGpeBJJEhZL0pRurmGXsyR_UKLY6tmyAG_E09cwoyOeNkga62bymTxH05oYn7_yyEpYTYwVEuxDkhCMxgPxkHWpaP9azs7T5ILnAs70Q_V1a9thMXyox_RBGyN5vtYxqjvOqeji534YfxCU44r2_YuQulIMhKK5htpHow15m_A7e4JoGwtiTWi-EpxVOnHZPafxUDrRf4PTDOqE5ILOSp0w3BcGgJGJhim-Oxq5627aQqBwSgqfNI2ikFB7XS3KbPW6ihBcxq1Py-_OColziwzD4fSq8h9eoPuCZ4WQNLKeFKjdiEbAb2oBGNypcOga5ApcweX2T2RsWMA7LxKgv5xSpLNHKhEBTErP5hjktgDPtPSG8SZxAhPREVyT30WXhvtaXds-NOFq8x2Ed9bvBfD26nfI79frEIiDhsuWHt2Oz7GjrApTrFRvlvw8QfLT9m_3jt4zfbiOCux35Xmt6QUBFtC6gknyoBNgoQ2l199l5owZ99Q-b7cuHYI4RWvvqgYERlJOpW1r09qXs6zAOJwAXMZVJtG_M_PGXzljT9PrFzVnrNoJmo3BYCoZ_i5X9lY33yhnHuiGVtZdnQiaeuppjNRGUCMFe3egDyVOkGGv-XvNDGw4mEh6P_k8tjOo9OLNIucU6002D1H8UHg2mSAZyy8Z5PT9JucgHTLtou2Iw2vyjWpQ_P-shPzDO21rnwHjS2dTh2-YBshiIrUWHtdtpy8oEQe6rxjQSRY2DBhgbDsoUMW9cVRpm2YVqBwxBfA11gvvLMUwtjXc3nsHvmQ5-2HvATWt2p9fI01-B2l7RqftJJCjni4J_Shacq24OD9Fy98KmX9CzRMCNKUKOVFhrwmHyAHQf8Ou5TkRc88xDnfATVvfjy100QgbhnFtEVdCQ7pii-0gCRX0IQQmkuwUIpe0J7N6sVIlXtYpQfaW1xHT0malV2rycXXRNxjJj-5Cs177de8v4G2k643DLyoSz3X_hFPi9Gb0qaJnOoCuFHXx_mvEcB_mC16Tq1qOVc8XjyN_M1PLSB3fbJ6ESj5WTPavQ8xmPfHueCu_td6TkHSBL8QtU_wG75T0ZZlDN-zohsGYbMUe35IwIdxI2Qtfcbq8jAcj6ElutyKVLKaYTyphosVysCra-cRyoFSws8OM1BTAof-169T_BIZPq8pY3Vziehn8ZgLBSop6hAmQztTgl5R6DhWdto62aeW8zW0wGaopffIVA2LsQruCnqPOqRCq31trukqTXR3o9c59F7l1C_SljlQvubcrvPmK3fQGs-vwwtAi1f4-udT02-sNAgp9fBkAOfAwjvXf7lLCte9EqLqxwUtdh1fq80t0uh-SlJ0fqauzaV9iwa-Cd9b40w=w1199-h911"
        };

        var pet2 = new Pet
        {
            Name = "Whiskers",
            Species = Species.Cat,
            Birth = new DateTime(2022, 5, 10).ToUniversalTime(),
            Gender = Gender.Female,
            IsNeutered = true,
            Description = "Playful cat that loves to cuddle.",
            PictureLink = "https://lh3.googleusercontent.com/fife/ALs6j_GaRU0aQEs5kyNdv5DJNszjnQ_PbOWAx0mDUglgwu1lB2MfCi5V3zR1W5fOLQH_1h3dTc5ryT8RN4L-wFsq4VGb-rlwA-eCRBAJRRLejlZqHfEG9hLiZgAI0K31GiOsLygYd0FxY7SQK61E-NXZQyJHwLi0e26U2TEnhqVoM2gaP6hMHqZf4fgOckD6imNuLeOIDYAhTRe1SGwsXrNONGlR6tOr-0358PFdHINGZjYRg2wA5DEG_vJwCcUuvMN1p52EHTezKCZTTVrnwcrtqTKMGa71FLWH8AMwTw6iCIFGYx3MJLwqjmnmkKhp35XvikUsm6em1d8AYu_u0i2iKnOHfkXlaAF5tf8OXr321-kPtR02ydHMZn08B9t28fRAoQKOpvPcduxCoej2-Bai5Vfiaz0wciktkVrq0Px8BstsAQkZ-qhbrAG11gsqA2P5gbcjAkV2VURZPS963DznoG4ypdhSdoRxb44fai2zYgC6AsbNdSyj5YwT4RyY3ijW_CjC2BkmcqBMnHdSC48fFsfIrLqY8P9niWub69QCVaXJF-BNjFbrDnoLOreQBRHLMoEry2xABl-zPUhen4T4LKLkkN1HgGdw-a7FvvctYaPueghzGwLE24JLilZyxvIPZm_bw6miOgUo6k79x7uQyPteUeHXjpYcTDtkeYQjmIU66Uhj3EV-S70N8hkdpvYNapgPiANlR_X7_BqsFX21AneRobi68GvDHtDQzh0-aINGBG7Vs_LuzEnjpWrVc0Ii8-e9ZRrnnDwsch75Ygie_iELLK3wjAS8VDH7gxQgM5QL5k6tXcgqovdr6x6YNqEI7RCq09izMtdwYfRx2dJ8iLFUyUgviLbE7gbkkM5EKfdm0zij27Khqnsk5sIWg_c4W_RamKUL0VR7hBfK3jmbZWul3tqS7DEFa9z4A8vcwML2TQTeGoNgU4b00POzKAZQajq0uvzIZ2kxgInZF2fMp28SEL-9DBvcxgXxzf0kynuwl_fHDmSVKVDrctgksvJA9mOgupA2hlLC4tWP9QWLU3BL7_UBn7nvaF4LbQXP66RPqhl85OmLyAB2SEsv6_dTgkRIkppjEX3lZcXELcIEuyRXZ_chp1rVb5Mx6uVMvTwwiDehLxyOvt-u0qJSN-A5rmfd4R2vn7xlWXjtVcJW6ITMeGW346MiNyi2f3uldOlh4qwSeyn2fDVG8R7PrZ3esBSLsVCwIHv5heDJpHmyYbLmEUY4Y1mL8MzrrKZfeYCaW9gq175zRVQ6q897DWrI8XglD732D7iYd6OIPFAKZ0ohGdjTC5s4_5kNzpSqy3br4kv2UGa3MgOZ15AiYpvd9krOOD-2vecojIL6C9Tq3tGwS8xMQfVPN-yiOTXkVunGf1bLkZM93OHtR9_L-2n9lh0Nr8ffKpKTiakHPLv0KIyUdOvZ9mLUkz-0kzmxqoQZqO-FNpYJL_6JEtQz2XDnxTrx73tDWDr6ljMAkTbufbFDIP5jZNgQ4nsj8rBAarbW7uDj8NBeg4Fx-5WSEOdvswH-j_FKIhr1xDg3IkbPhmzpX94FZdRiuAAQ2TG1Z_WgbVybZEimS9kaH7A8lwUMpQX1hIJXQRali8tR3VaLtJWs2T4CUMR1JaTVpt4WMTpyKWGlA7zdkA=w1199-h911"
        };
        
        var pet3 = new Pet
        {
            Name = "Clover",
            Species = Species.Rabbit,
            Birth = new DateTime(2014, 5, 31).ToUniversalTime(),
            Gender = Gender.Female,
            IsNeutered = false,
            Description = "Lively and curious rabbit. Loves hopping around its enclosure and munching on hay.",
            PictureLink = "https://lh3.googleusercontent.com/fife/ALs6j_GPDLojBmVprDnrQY7qRq7-LOX4VZbM_FKDbbOCSjA3Atyyt4Mz36Du7HrosYRiBJooCiYjmoMDOo31OTFARmi05YsLgKsVxxvit610hlFloeLYBWsqYdo9kKqjsUBbeqpOWQw0Qoob7-mayKOCk8ztkG2PONPWsmCibmCu2zPiy_EawMJpYkzFL1qtrj27LeqkxQJlR_zQ5QsKcDcsFCboWRxOJL5z1izHwJ1bCB2dMsVENvRcnqCpQ3w0eUNZ0k1GE40VeFI9eI-KOXjo8v4TIN3CtlqPmMliPAk3JBQwFDEFDVhx6KNb9MYkrwJtdbTVuUhg0lBUl6KKQae9RqCwoFW1GHQ_XyewL88ALw1I4gFBuSoJzqT-fumQgOayQC8H-f63i5AUwkDGLaiMKCdcK8joyPRzO3Sf_-RMImqoOBGXAkMjmIU4yUL7kukn0z-M5tDUvQdCqopIreEUjo2G9A9kd4jnicUwoIzNBYa-QtX6GV7dQ_ldnUH3gCamMVfyBXrDstQToAHUUQcaGI5JfJAfuwWVye-HqcETwjAv0Nnif8e_ndSdfOY7OTeNa0zvHpxuIRH6WlO48saswDHeYvfI2XJ6t0rCPeSLJhW_WFtlwqNfWfwxIdGA03hKN72hSvQBmG58nsVXBl_SahOdEyW0ujBl7OWZNJXRluA0Lu97RTt0pCI9Nr_OswElM0-a8HtDi3-rssy-8ZzdBmnxorMox5aFoFmpnrYFI55PLHr8YZucsx_wgku31tgJHHJkbXWn2-laqLPd5Dddx7Xok94VODYRdLCrAjniOnTSBmgnp-l0RpgK0OBz7QLGRPSKfCO7rVu4lsGT2hj0uHK4VrjsKclT7CCkgqiVtTwvq-26S8PT4YJlPYY2U9vffNN7age3BuKzNlr3RHXwA6S4_Bl6hNzwAUHKA5BuMmZO6pc4FBLR3ZBMU-e8Bz55nqQ2vog8SvhH-nXrfzKIvdnWmN7LNJcLPfPByRfTRs8xm6aBiYj1Sik6vhYWJIAIsi70AQ3n2qqvOpq3aiFtII9nw9KYr_Ynp9vUZ2tY92SGgqimV7-Yl6XENIq54GSR_s84fK5Esa8BW69iQ4lyYvVowtfuk4UkLc-jUZzxTyZLUjR5f8es7OH5owbexPXPC6StdGXdWKafU459Hq7E9LkdVO1YCRHRc7lDQwR_IhD9JJ0Kn0QChFKAYf_yM5AkSWhP0JaIP6-2fkIu6wZsOY6uX2co6RdgiW36hg4Xyfz9F6_r8tXcJp6l8NfsJylwoAvvbidq4gewqRichdFWkWCqZVkIC_azdkICYFGRAF4liGuFOkB7dJ-w5GEP5MoqEtFD7NHzuiYOiC_Fbh55vPJ9Ee4YUDpF6Z5lvKXi30gQ3NKMcFKwfCKGL4yVzbN0wxcXbN8S-v5T1XSQtD8zF-1SlX5oVm4mXy7p1WHbUUHdlI_1G-Er1agr9VesTVCC7QfvAjWKJG6JnOhCb9s7vpPxjFbq-pG2fk8Pq9cnCN0l2YMUmwdtyiuuxmJ-Ph25X5IFDMDbxLEs3VRm8ixe5BNbqlLUucR-N9d81nNF3RJMqF9iFSLpOMxSQbEDjvF1uAQTa2RRpuMC4YGBd3lxRtTVIRVKZ0BGPJMc_Viu_a-C-JHAapUh4g=w10000-h10000"
        };
        
        var pet4 = new Pet
        {
            Name = "Shadow",
            Species = Species.Cat,
            Birth = new DateTime(2019, 2, 14).ToUniversalTime(),
            Gender = Gender.Male,
            IsNeutered = false,
            Description = "Independent and curious cat. Loves exploring new environments.",
            PictureLink = "https://lh3.googleusercontent.com/fife/ALs6j_EsjlEgu8d0cuQ9NR74U5jj4qFH9nq9k-u7Mqvtzau8lhj0Oo-nrFZ97UdcyCQPtrOYiT_uaTXU2zXXPfebwFFy53WmBRv9IgLM85JbJBwRBrMR9hFF1Wg9LBCwxWXLTD6PbfMzk0oW5AUxBaxbF03t-D9Vvr_VayonRgoZYtD3hUZPlU7J7Rsftrl53EKKNl6yrNNVH4anWTZVtAY1Pd31wy2yjeqaYjh-jgV75QpLJ0Dmf1x-T5lq_jIl5-a2KtPPyT3aAvwe7BUmlG_5ll6hxa2BRG7yC0nzNBYhT5E3IK40Fk2eWvZ_4pN-wvpXK0ZbvHw-JczVTLv1joiepthx3raEsFBvLoEY6f2CVS_S0i9S7vEX0WXGWPQ3sFMTc89aLKkI2yKKFA7ge4oHE8DdydVyTNCGxSpgH8LofdUyZ1tEuDWibM3j9Zij10QfRvMOHE-lm1L9FUHJAJfenH2V26MEtviwh86Hmpotso-8dOsADpZy_EmbhDk5aAMVlwLrLSHTCrRe1FYnTmlzPUuT9qnH_Z7GIyhh-X_X6zYvNlaL0b1izW_JlIRVEmAe0Ux1fGMtRL_2fEsktf6QddWSYsH40-SkAtvw-w5qq7HSC_yuHAAByOzY3KCPgoRNHwuzPj-0nWuyhYVASuITSlrKa7E52gLdD-BvbEk5GRjdURXHsT5DfH_OgXsdASvetlxubS8zrdsX-4ezxM17TTQrL72D9ETYe2XWwez-yo1_jBoWNBGDf8pAQTSaKcqxjrJaPImueZftip3uSgLzcn8J6EvW5rSomIEqdbROk6lL7pVRSpFK64YbL9DxNTxIBPrywMcBBNTHuVIFc-VDPiuwU9NhMgOGLV-aqQAIFM59xf1TOitux6KkB047hicVa6VdB2TmH2Nyb-3mroQ3nL0opjYUUs5Q_JvJxBboh7Ge-GKes1cWkSgmHRLPM6YYGY5hEOQKnax-3hiBtYydLHZbUWcA7haIvz1RdVyaH479ELjQXwhP4qJWW7jVgdRZSR9iEIY9b1LSYXz1Z5ItjXdX_V2y03JEH9Tv_A4-PQETIkSE-Bs08sFOJ2KNtgYsd3BMCdzPat3yuArNNd0R2PWoBHLki4n5DU15SPEPdV703Ku78RY5CHArVwbO7Y6innX46PKy9ZLfSSrcE63T95wmWEVqP-1NrQ2cBuTch1F8J3u7-NYOI522cSraLqKjdMCL5f2WPMuzNQyZ0E_Eat6KkPiFmFAwzlE8d0Q9kpL1FhfoQpWY7LHLaXrP4jHt-6hcFa20bVKJ8CkIRxiX-Vj1ZXO_rsct-cQvKZYge4xAEy2eJSO_b3KU2TMjy0xZzGkwdUEGFtAasOKH9wc4H_fGdHxPoqy3dfoNTkdHkFgAAay6Z81oB4aEOtdAymiI8hXo77v5D7phn0WtJxEr-isohD-C7yBI5OjZa3wl-q-LPe6W2lN41NQeLewP15X1HJ7_Ix0aJooYvvMuF1Radz135f0u963H_4jZGlQBYUmGfOVevV9czHszxtF7dIq9LsFmmbBbrjKqOf3i6Z7_F4j-FVVrxm-3XZV6UvFQPreaaLEHSBnH-MyKLWqB8tPOGHvUbtR8f3MKZDE663Q2-KvCcXWFmjvdsEWE0umfzbwFHJONjZNiIzQ=w1199-h911"
        };

        dbContext.Pets.AddRange(pet1, pet2, pet3, pet4);
        dbContext.SaveChanges();

        var ad1 = new Advertisement
        {
            PetId = pet1.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMonths(1)
        };

        var ad2 = new Advertisement
        {
            PetId = pet2.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMonths(1)
        };
        
        var ad3 = new Advertisement
        {
            PetId = pet3.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(10)
        };

        dbContext.Advertisements.AddRange(ad1, ad2, ad3);
        dbContext.SaveChanges();
    }
}

