"use client";

import Link from "next/link";
import { useLanguage } from "../context/LanguageContext";
import { Globe, Ticket, Users, Building, ShieldCheck, LogIn, LogOut, LayoutDashboard } from "lucide-react";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

export default function Navbar() {
  const { t, locale, setLocale, dir } = useLanguage();
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem("auth_token");
    setIsAuthenticated(!!token);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("auth_token");
    setIsAuthenticated(false);
    router.push("/login");
  };

  return (
    <nav className="bg-white shadow border-b border-gray-200">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          <div className="flex items-center space-x-4 rtl:space-x-reverse">
            <Link href="/" className="flex items-center space-x-2 rtl:space-x-reverse text-indigo-600 font-bold text-xl">
              <ShieldCheck className="h-7 w-7" />
              <span>{t("appName")}</span>
            </Link>
            <div className="hidden md:flex space-x-4 rtl:space-x-reverse ms-8">
              <Link href="/" className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600 px-3 py-2 rounded-md text-sm font-medium">
                <LayoutDashboard className="h-4 w-4 me-1" />
                <span>{t("dashboard")}</span>
              </Link>
              <Link href="/tickets" className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600 px-3 py-2 rounded-md text-sm font-medium">
                <Ticket className="h-4 w-4 me-1" />
                <span>{t("tickets")}</span>
              </Link>
              <Link href="/organizations" className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600 px-3 py-2 rounded-md text-sm font-medium">
                <Building className="h-4 w-4 me-1" />
                <span>{t("organizations")}</span>
              </Link>
              <Link href="/sla" className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600 px-3 py-2 rounded-md text-sm font-medium">
                <ShieldCheck className="h-4 w-4 me-1" />
                <span>{t("slaPolicies")}</span>
              </Link>
              <Link href="/users" className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600 px-3 py-2 rounded-md text-sm font-medium">
                <Users className="h-4 w-4 me-1" />
                <span>{t("users")}</span>
              </Link>
            </div>
          </div>
          <div className="flex items-center space-x-4 rtl:space-x-reverse">
            {/* Language Switcher */}
            <div className="flex items-center bg-gray-100 rounded-lg p-1">
              <Globe className="h-4 w-4 text-gray-500 mx-1" />
              <button
                onClick={() => setLocale(locale === "en" ? "fa" : "en")}
                className="text-xs font-semibold px-2 py-1 rounded bg-white shadow text-gray-700 hover:text-indigo-600 transition"
              >
                {locale === "en" ? "فارسی" : "English"}
              </button>
            </div>

            {isAuthenticated ? (
              <button
                onClick={handleLogout}
                className="flex items-center space-x-1 text-red-600 hover:text-red-800 px-3 py-2 rounded-md text-sm font-medium"
              >
                <LogOut className="h-4 w-4 me-1" />
                <span>{t("logout")}</span>
              </button>
            ) : (
              <Link
                href="/login"
                className="flex items-center space-x-1 bg-indigo-600 text-white hover:bg-indigo-700 px-3 py-2 rounded-md text-sm font-medium shadow"
              >
                <LogIn className="h-4 w-4 me-1" />
                <span>{t("login")}</span>
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
