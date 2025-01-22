export function isDateString(value: any) {
  if (typeof value !== "string") return false;
  const dateStringRegex =
    /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|([+-]\d{2}:\d{2}))?$/;
  if (!dateStringRegex.test(value)) return false;
  return !isNaN(Date.parse(value));
}
