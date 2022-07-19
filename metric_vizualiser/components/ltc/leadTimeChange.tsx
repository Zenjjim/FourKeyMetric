import * as Plot from "@observablehq/plot";
import { useEffect, useRef } from "react";
import { ILeadTimeChange } from "types";
import { getDateOfWeek } from "utils";
type LeadTimeChangeProps = { data?: ILeadTimeChange; months: number };

export function LeadTimeChange({ data, months }: LeadTimeChangeProps) {
  const headerRef = useRef(null);

  useEffect(() => {
    if (data === undefined) {
      return;
    }

    const getMontlyLeadTimeChange = (data: ILeadTimeChange) =>
      data.monthlyLeadTimeChange.map((d) => ({
        date: new Date(d.key.yearNumber, d.key.monthNumber),
        median: d.median / 3600,
      }));
    const getWeeklyLeadTimeChange = (data: ILeadTimeChange) =>
      data.weeklyLeadTimeChange.map((d) => ({
        date: getDateOfWeek(d.key.weekNumber, d.key.yearNumber),
        median: d.median / 3600,
      }));

    const interval =
      months < 4 ? data.weeklyLeadTimeChange : data.monthlyLeadTimeChange;
    const transformedDataMedian =
      months < 4
        ? getWeeklyLeadTimeChange(data)
        : getMontlyLeadTimeChange(data);

    const transformedData = data.changes.map((d) => ({
      date: new Date(d.startTime * 1000),
      time: (d.finishTime - d.startTime) / 3600,
    }));

    const chart = Plot.plot({
      style: {
        background: "transparent",
      },
      y: {
        grid: true,
        label: "Hours",
      },
      color: {
        type: "diverging",
        scheme: "burd",
        range: ["red", "blue"],
        interpolate: "hcl",
      },
      marks: [
        Plot.ruleY([0]),
        Plot.line(transformedDataMedian, {
          x: "date",
          y: "median",
          curve: "basis",
          marker: "circle",
          stroke: "steelblue",
          opacity: 1,
        }),
        Plot.dot(transformedData, {
          x: "date",
          y: "time",
          fill: "steelblue",
          fillOpacity: 0.75,
          interval: interval,
        }),
      ],
    });
    // @ts-ignore
    headerRef.current.append(chart);
    return () => chart.remove();
  }, [data, months]);

  return <div ref={headerRef}></div>;
}
