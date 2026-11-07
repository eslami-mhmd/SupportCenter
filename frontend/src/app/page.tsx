"use client";

import { useLanguage } from "../context/LanguageContext";
import Link from "next/link";
import { Ticket, Building, ShieldCheck, Users, ArrowRight } from "lucide-react";

export default function Home() {
  const { t, dir } = useLanguage();

  return (
    <div className="space-y-8">
      <div className="bg-gradient-to-r from-indigo-600 to-violet-600 rounded-2xl p-8 text-white shadow-xl">
        <h1 className="text-3xl font-extrabold mb-4">{t("appName")}</h1>
        <p className="text-indigo-100 max-w-2xl text-lg">
          {dir === "rtl"
            ? "پلتفرم پشتیبانی مشتریان چند‌سازمانی با قابلیت مدیریت تیکت‌ها، سازمان‌ها، سیاست‌های SLA و کاربران."
            : "Multi-tenant customer support platform for managing tickets, organizations, SLA policies, and users efficiently."}
        </p>
        <div className="mt-6 flex space-x-4 rtl:space-x-reverse">
          <Link
            href="/tickets"
            className="bg-white text-indigo-600 font-semibold px-6 py-3 rounded-lg shadow hover:bg-indigo-50 transition flex items-center space-x-2 rtl:space-x-reverse"
          >
            <span>{t("tickets")}</span>
            <ArrowRight className="h-5 w-5 rtl:rotate-180" />
          </Link>
          <Link
            href="/tickets/new"
            className="bg-indigo-700 text-white font-semibold px-6 py-3 rounded-lg shadow hover:bg-indigo-800 transition"
          >
            {t("createTicket")}
          </Link>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Link
          href="/tickets"
          className="bg-white p-6 rounded-xl shadow border border-gray-100 hover:shadow-md transition flex flex-col justify-between"
        >
          <div>
            <div className="p-3 bg-indigo-50 text-indigo-600 rounded-lg w-fit mb-4">
              <Ticket className="h-6 w-6" />
            </div>
            <h3 className="text-lg font-bold text-gray-900 mb-1">{t("tickets")}</h3>
            <p className="text-sm text-gray-500">
              {dir === "rtl" ? "مدیریت و پیگیری تیکت‌های پشتیبانی" : "Manage and track support tickets"}
            </p>
          </div>
          <span className="mt-4 text-indigo-600 font-medium text-sm flex items-center">
            {dir === "rtl" ? "مشاهده فهرست ←" : "View List →"}
          </span>
        </Link>

        <Link
          href="/organizations"
          className="bg-white p-6 rounded-xl shadow border border-gray-100 hover:shadow-md transition flex flex-col justify-between"
        >
          <div>
            <div className="p-3 bg-emerald-50 text-emerald-600 rounded-lg w-fit mb-4">
              <Building className="h-6 w-6" />
            </div>
            <h3 className="text-lg font-bold text-gray-900 mb-1">{t("organizations")}</h3>
            <p className="text-sm text-gray-500">
              {dir === "rtl" ? "مدیریت سازمان‌ها و تننت‌ها" : "Manage organizations and tenants"}
            </p>
          </div>
          <span className="mt-4 text-emerald-600 font-medium text-sm flex items-center">
            {dir === "rtl" ? "مشاهده سازمان‌ها ←" : "View Organizations →"}
          </span>
        </Link>

        <Link
          href="/sla"
          className="bg-white p-6 rounded-xl shadow border border-gray-100 hover:shadow-md transition flex flex-col justify-between"
        >
          <div>
            <div className="p-3 bg-amber-50 text-amber-600 rounded-lg w-fit mb-4">
              <ShieldCheck className="h-6 w-6" />
            </div>
            <h3 className="text-lg font-bold text-gray-900 mb-1">{t("slaPolicies")}</h3>
            <p className="text-sm text-gray-500">
              {dir === "rtl" ? "تنظیم سیاست‌های توافق سطح خدمت" : "Configure SLA policies & rules"}
            </p>
          </div>
          <span className="mt-4 text-amber-600 font-medium text-sm flex items-center">
            {dir === "rtl" ? "مشاهده SLAها ←" : "View SLA Policies →"}
          </span>
        </Link>

        <Link
          href="/users"
          className="bg-white p-6 rounded-xl shadow border border-gray-100 hover:shadow-md transition flex flex-col justify-between"
        >
          <div>
            <div className="p-3 bg-purple-50 text-purple-600 rounded-lg w-fit mb-4">
              <Users className="h-6 w-6" />
            </div>
            <h3 className="text-lg font-bold text-gray-900 mb-1">{t("users")}</h3>
            <p className="text-sm text-gray-500">
              {dir === "rtl" ? "مدیریت کاربران و دسترسی‌ها" : "Manage users and roles"}
            </p>
          </div>
          <span className="mt-4 text-purple-600 font-medium text-sm flex items-center">
            {dir === "rtl" ? "مشاهده کاربران ←" : "View Users →"}
          </span>
        </Link>
      </div>
    </div>
  );
}
