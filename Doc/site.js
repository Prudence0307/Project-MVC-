document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("form[onsubmit]").forEach(form => {
    form.addEventListener("submit", event => {
      // Confirmation prompts are provided inline by the relevant form.
    });
  });
});
