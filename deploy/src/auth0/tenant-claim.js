function (user, context, callback) {
    if (context.clientName.startsWith("{{ app_environment }}:")) {
        context.accessToken[`app/tenants`] = user?.app_metadata?.tenants || [];
    }

    return callback(null, user, context);
}