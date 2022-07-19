export function getDateOfWeek(w: number, y: number) {
  const d = 1 + (w - 1) * 7; // 1st of January + 7 days for each week

  return new Date(y, 0, d);
}

export const getHoursfromSeconds = (seconds: number) => seconds / 3600;

export const getDaysfromSeconds = (seconds: number) => (seconds / 3600) * 24;
