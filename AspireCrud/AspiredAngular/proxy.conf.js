module.exports = {
  "/api": {
    target:
      process.env["services__weatherapi__https__0"] ||
      process.env["services__weatherapi__http__0"] ||
      "https://localhost:7577",
    secure: false,
    changeOrigin: true,
    pathRewrite: {
      "^/api": "",
    },
    logLevel: "debug"
  },
};
